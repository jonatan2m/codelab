var http = require('http'),
	assert = require('assert');

var opts ={
	host: 'localhost',
	port: 8000,
	path: '/send',
	method: 'POST',
	headers: {'content-type': 'application/x-www-form-urlencoded'}
};

var req = http.request(opts, (res) => {
	res.setEncoding('utf8');

	var data = "";
	res.on('data', (d) => { 
		data += d;
	});

	res.on('end', () => {
		assert.strictEqual(data, '{"status":"ok","message":"Tweet received"}');
	});
});


req.write('tweet=test');
req.end();