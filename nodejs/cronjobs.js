var cron = require('node-schedule');
var rule = new cron.RecurrenceRule();
rule.second = 15;

var job = cron.scheduleJob(rule, () => {
	console.log(new Date());
	console.log('Job is executing');
});