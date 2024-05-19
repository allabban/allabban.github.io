<?php
$dname = "G231210563@sakarya.edu.tr";
$dpassword = "G231210563";

$name = $_POST['name'] ?? '';
$password = $_POST['password'] ?? '';

if ($name === $dname && $password === $dpassword) {
     $Message = "g231210563, Hoş Gelidiniz";
    echo "<script type = 'text/javascript'> alert('$Message');</script>";
    echo "<script type='text/javascript'>setTimeout(function(){ window.location.href = 'https://allabban.github.io'; }, 500);</script>";
    exit; 
} else {
     $FailMessage = "Lütfen email ve password kontrol edin.";
    echo "<script type = 'text/javascript'> alert('$FailMessage');</script>";
    echo "<script type='text/javascript'>setTimeout(function(){ window.location.href = 'https://allabban.github.io/login.html'; }, 500);</script>";
    exit;
}
?>