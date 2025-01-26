

const Api = async (url, method, data, token)=>{
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json" 
        },
        body: JSON.stringify(data) 
    };
    try {
        const response = (await fetch(url, options))
        if (response.status != "401" && response.status != "403") {
            console.log(response.status)
            const json = (await response.json())
        return json;
    }else{
        window.location.href ="/login.html"
    }
    } catch (ex) {
        return ex
   }
}

const GetApi = async (url, method, token)=>{
    const options = {
        method: method,
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json" 
        },
    };
   try{
       const response = (await fetch(url, options))
       console.log(response.status)
       if (response.status != "401" && response.status != "403"){
           const json = (await response.json())
        return json;
    }else{
        window.location.href ="/login.html"
    }
    
   } catch (ex) {
       return ex
   }
}