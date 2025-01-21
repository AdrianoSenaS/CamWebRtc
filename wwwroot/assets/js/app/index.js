
navigator.mediaDevices.getUserMedia({video:true, audio:true})
navigator.mediaDevices.enumerateDevices().then(e=>{
    let trakid=0;
    let videoId =0
    e.forEach(tracks=>{
      if(tracks.kind === "audioinput" || tracks.kind =="audiooutput"){
        trakid++
        document.getElementById("outrosDispositivos").innerText = trakid
      }
      if(tracks.kind === "videoinput"){
        videoId++
        document.getElementById("totaldecameras").innerText = videoId
      }
    })
})








