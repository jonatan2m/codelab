//https://nodejs.org/api/worker_threads.html

async function fibonacci(n) {
    return n < 1 ? 0
        :  n <= 2 ? 1
        : await fibonacci(n-1) + await fibonacci(n-2)
}

async function doFib(iterations) {
    const start = Date.now();
    const result = await fibonacci(iterations);
    console.log(`doFib done in: ${Date.now() - start}ms`);
    return result;
}

async function main(){
    const start = Date.now()

    const values = await Promise.all([
        doFib(40),
        doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40),
        //doFib(40)
    ])

    console.log('values', values);

    console.log(`[main] done in: ${Date.now() - start}ms`);
}

main().catch(console.error);