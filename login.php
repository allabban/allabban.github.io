<?php
if (isset($_GET['ename'])){
    echo "Welcome :". $_GET["ename"]."<br>";
}else{
    header("location: https://allabban.github.io/login.html")
}