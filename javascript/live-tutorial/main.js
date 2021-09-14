if(window.Worker){
    const worker = new Worker('run-code-worker.js');   
    worker.onmessage = function (e){
        document.querySelector('#code').innerHTML = e.data;
        console.log('Message received from worker');
    }

    document.querySelector('.set-code').addEventListener('click', function (){
        editor.setValue("\
    function foo(x){\n\
        console.log('teste', x > 5);\n\
        return x > 5 ? 'é maior que 5' : 'é menor que 5'\
    }");    
    });
    
    document.querySelector('.get-code').addEventListener('click', function (){
        document.querySelector('#code').innerHTML = editor.getValue();
    });
    
    document.querySelector('.exec-code').addEventListener('click', function (){
        const userCode = "(" + editor.getValue() + ")";        
    
        //worker.postMessage(userCode);    

        // setTimeout(function(){
        //     worker.terminate();             
        // }, 5000);        

        var t = eval("(" + editor.getValue() + ")");
        t(6);

        //--------------------------

        // const blob = new Blob([userCode], {
        //     type: 'text/javascript'
        // });
        // const blobUrl = URL.createObjectURL(blob);
        
        // let worker2 = new Worker(blobUrl);

        // worker2.addEventListener('message', function (e){
        //     document.querySelector('#code').innerHTML = "RESULT: " + (e.data).toString();
        // }.bind(this));

        // worker2.addEventListener('error', function(e){
        //     document.querySelector('#code').innerHTML = "ERRO: " + e.toString();
        // }.bind(this));

        // worker2.postMessage("start");

        // setTimeout(function(){
        //     worker2.terminate();
        //     worker2 = null;
        // }, 5000);

        
        //--------------------------
    
        /*try{
            var func = new Function(userCode);
            var result = func(2);
            document.querySelector('#code').innerHTML = result;
        }
        catch(err){
            document.querySelector('#code').innerHTML = err;
            console.error(err);
        }
        */
    });
}else{
    alert("This browser can't execute our code. Please, upgrade your browser version or install a newer one");
}

