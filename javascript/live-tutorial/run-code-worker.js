onmessage = function(e){
    console.log('Message received from main script', e);
    
    var result = `Result: ${eval(e.data)(6)}`;
    
    console.log('Posting message back to main script');
    postMessage(result);
}