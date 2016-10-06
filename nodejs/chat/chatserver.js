var net = require('net');

var chatServer = net.createServer(),
    clientList = [];

chatServer.on('connection', (client) => {
	client.name = client.remoteAddress + ":" + client.remotePort;	
	client.write('Hi ' + client.name + '\n');

	clientList.push(client);
	
	client.on('data', (data) => {
		for(var i = 0; i < clientList.length; i++){
			//console.log(data.toString());			
			broadcast(data, client);
		}

	});
	
	client.on('end', () => {
		console.log(client.name + " quit");
		clientList.splice(clientList.indexOf(client), 1);
	});

	client.on('error', (e) => {
		console.log(e);
	});
	
});

function broadcast(message, client){
	var cleanup = [];

	for (var i = 0; i < clientList.length; i++) {
		if(client !== clientList[i]){
			if(clientList[i].writable){
				clientList[i].write(client.name + " says " + message);				
			}else{
				cleanup.push(clientList[i]);
				clientList[i].destroy();
			}
		}
	}
	//Remove dead Nodes out of write loop
	for (var i = 0; i < cleanup.length; i++) {
		clientList.splice(clientList.indexOf(cleanup[i]), 1);
	}
}

chatServer.listen(9000);