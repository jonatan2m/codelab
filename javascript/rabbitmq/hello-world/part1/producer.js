var amqp = require('amqplib/callback_api');
amqp.connect('amqp://localhost', function (err, conn) {
    conn.createChannel(function (err, ch) {
        var q = 'hello';

        ch.assertQueue(q, { durable: false });
        //ch.sendToQueue(q, new Buffer('Hello World!'));
        var msg = JSON.stringify({ msg: 'Hello World!' });
        ch.sendToQueue(q, new Buffer(msg));
        console.log(" [x] Sent 'Hello World!'");
    });
    setTimeout(function () { conn.close(); process.exit(0) }, 500);
});