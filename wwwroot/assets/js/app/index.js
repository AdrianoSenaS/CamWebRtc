//P�gina inicial rascunho de teste
navigator.mediaDevices.getUserMedia({video:true, audio:true})
navigator.mediaDevices.enumerateDevices().then(e=>{
    let trakid=0;
    let videoId =0
    e.forEach(tracks=>{
      
      if(tracks.kind === "videoinput"){
        videoId++
        document.getElementById("totaldecameras").innerText = videoId
      }
    })
})








