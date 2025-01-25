const username = document.getElementById("username")
const password = document.getElementById("password")
const url = "/api/Auth/login"

document.getElementById("BtnLogin")
.addEventListener('click', (e)=>{
    e.preventDefault()
    const data = {
        "username": username.value,
        "password": password.value
    }
    const response = Api(url, "POST", data, "")
    response.then((e)=>{
       if(e.token != null){
        const expirationDate = new Date();
        expirationDate.setTime(expirationDate.getTime() + 60 * 60 * 1000);
        document.cookie = `token=${e.token};  expires=${expirationDate.toUTCString()}; path=/; SameSite=Strict;`;
        window.location.href = "/home.html"
       }
    })
})


