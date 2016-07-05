var express = require('express');

var app = express();

app.get('/', (req, res) => {
    res.send('Welcome to Node Twitter');
});

app.listen(8000);
