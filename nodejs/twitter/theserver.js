var express = require('express');
var bodyParser = require('body-parser');
var app = express();
app.listen(8000);

var tweets = [];

app.get('/', (req, res) => {
    res.send('Welcome to Node Twitter');
});

app.post('/send', bodyParser.urlencoded({ extended: false }), (req, res) => {
	console.log(req.body);
	if(req.body && req.body.tweet){
		tweets.push(req.body.tweet);
		res.send({status: "ok", message: "Tweet received"});
	}else{
		res.send({status: "nok", message: "No tweet received"});
	}
});

app.get('/tweets', (req, res) => {
	res.send(tweets);
});
