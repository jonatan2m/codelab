var net = require('net');

var chatServer = net.createServer();
chatServer.on('connection', (client) => {
	client.write('Hi\n');
	client.write('Bye!\n');

	client.end();
});

chatServer.listen(9000);