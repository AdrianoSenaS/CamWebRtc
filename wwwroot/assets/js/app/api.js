

const Api = async (url, method, data, token)=>{
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json" 
        },
        body: JSON.stringify(data) 
    };
   try{
    const response = (await fetch(url, options)).statusText
    if(response != "Unauthorized" && response != "Forbidden"){
        const json = (await fetch(url, options)).json()
        return json;
    }else{
        window.location.href ="/login.html"
    }
   }catch(ex){
    return ex;
   }
}

const ApiGet = async (url, method, token)=>{
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json" 
        },
    };
   try{
    const response = (await fetch(url, options)).statusText
    if(response != "Unauthorized" && response != "Forbidden"){
        const json = (await fetch(url, options)).json()
        return json;
    }else{
        window.location.href ="/login.html"
    }
    
   }catch(ex){
    return ex;
   }
}