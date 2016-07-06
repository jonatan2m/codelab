// server.js
// load the things we need

//tutorial: https://scotch.io/tutorials/use-ejs-to-template-your-node-application#disqus_thread

var express = require('express');
var engine = require('ejs-mate');
var app = express();


// use ejs-locals for all ejs templates: 
app.engine('ejs', engine);

console.log(__dirname);

app.set('views',__dirname + '/views');

// set the view engine to ejs
app.set('view engine', 'ejs');

// use res.render to load up an ejs view file

// index page 
app.get('/', function(req, res) {

	var drinks = [
        { name: 'Bloody Mary', drunkness: 3 },
        { name: 'Martini', drunkness: 5 },
        { name: 'Scotch', drunkness: 10 }
    ];
    var tagline = "Any code of your own that you haven't looked at for six or more months might as well have been written by someone else.";

    res.render('pages/index', {
        drinks: drinks,
        tagline: tagline
    });
});

app.get('/new', function(req, res) {

	var drinks = [
        { name: 'Bloody Mary', drunkness: 3 },
        { name: 'Martini', drunkness: 5 },
        { name: 'Scotch', drunkness: 10 }
    ];
    var tagline = "With boilerplate...";

    res.render('pages/newindex', {
        drinks: drinks,
        tagline: tagline
    });
});

// about page 
app.get('/about', function(req, res) {
    res.render('pages/about');
});

app.listen(8080);
console.log('8080 is the magic port');