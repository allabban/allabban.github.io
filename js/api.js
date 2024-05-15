function GetBCO(){
  const myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer BQBRVKRa-QLOLNA8ly5NeXiY4TeDJyuVOSEIeHyMk6wxwrTgJLmjXXGH7y-E9IFtNB0UEp9ub33255wlPfExM1jx29wwRaPWsOF8ujTD46YigPRP8lA");

const requestOptions = {
  method: "GET",
  headers: myHeaders,
  redirect: "follow"
};

fetch("https://lichess.org/api/player", requestOptions)
  .then((response) => response.json())
  .then((result) => {
    const bestplayer1 = result.classical[0].username ;
    const bestplayer2 = result.classical[1].username ;
    const bestplayer3 = result.classical[2].username ;
    const bestplayrr1 = result.classical[0].perfs.classical.rating ;
    const bestplayrr2 = result.classical[1].perfs.classical.rating ;
    const bestplayrr3 = result.classical[2].perfs.classical.rating ;
    const bestbullet1 = result.bullet[0].username;
    const bestbullet2 = result.bullet[1].username;
    const bestbullet3 = result.bullet[2].username;
    const bestbulletr1 = result.bullet[0].perfs.bullet.rating;
    const bestbulletr2 = result.bullet[1].perfs.bullet.rating;
    const bestbulletr3 = result.bullet[2].perfs.bullet.rating;
    const bestblitz1 = result.blitz[0].username;
    const bestblitz2 = result.blitz[1].username;
    const bestblitz3 = result.blitz[2].username;
    const bestblitzr1 = result.blitz[0].perfs.blitz.rating;
    const bestblitzr2 = result.blitz[1].perfs.blitz.rating;
    const bestblitzr3 = result.blitz[2].perfs.blitz.rating;
    document.getElementById(`classicalone`).innerHTML = bestplayer1;
    document.getElementById(`classicaltwo`).innerHTML = bestplayer2;
    document.getElementById(`classicalthree`).innerHTML = bestplayer3;
    document.getElementById(`classicalrato`).innerHTML = bestplayrr1;
    document.getElementById(`classicalratt`).innerHTML = bestplayrr2;
    document.getElementById(`classicalrats`).innerHTML = bestplayrr3;
    document.getElementById(`bulletone`).innerHTML = bestbullet1;
    document.getElementById(`bullettwo`).innerHTML = bestbullet2;
    document.getElementById(`bulletthree`).innerHTML = bestbullet3;
    document.getElementById(`bulletrato`).innerHTML = bestbulletr1;
    document.getElementById(`bulletratt`).innerHTML = bestbulletr2;
    document.getElementById(`bulletrats`).innerHTML = bestbulletr3;
    document.getElementById(`blitzone`).innerHTML = bestblitz1;
    document.getElementById(`blitztwo`).innerHTML = bestblitz2;
    document.getElementById(`blitzthree`).innerHTML = bestblitz3;
    document.getElementById(`blitzerato`).innerHTML = bestblitzr1;
    document.getElementById(`blitzeratt`).innerHTML = bestblitzr2;
    document.getElementById(`blitzrats`).innerHTML = bestblitzr3;
  })
  .catch((error) => console.error(error));
}


