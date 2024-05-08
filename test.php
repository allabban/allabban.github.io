<?php

$url = "https://api.myanimelist.net/v2/users/Aaabdallah/animelist?fields=list_status&sort=anime_title&limit=100&offset=0";
$consumer_key = "db4efc0dc6a74e56b48f4287576d15d0";
$consumer_secret = "047d214b80c307026401b82a73a6ecfc3e3ea40ccd981f2e0fa883cbc628691a";

$headers = [
    "X-MAL-CLIENT-ID: $consumer_key",
    "Content-Type: application/json"
];

$method = "GET";
$curl = curl_init();

curl_setopt_array($curl, [
    CURLOPT_URL => $url,
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_CUSTOMREQUEST => $method,
    CURLOPT_HTTPHEADER => $headers,
    CURLOPT_USERPWD => "$consumer_key;$consumer_secret"
]);

$response = curl_exec($curl);

if ($response === false) {
    echo "Error: " . curl_error($curl);
} else {
    $jsonObj = json_decode($response, true);
    if (isset($jsonObj['data'])) {
        foreach ($jsonObj['data'] as $item) {
            $node = $item['node'];
            if (isset($node['id'], $node['title'], $node['main_picture']['medium'])) {
                $anime_id = $node['id'];
                $title = $node['title'];
                $medium_picture_url = $node['main_picture']['medium'];

                echo "ID: $anime_id <br>";
                echo "Title: $title <br>";
                echo "Large Picture URL: $medium_picture_url <br>";
                echo "<img src=\"$medium_picture_url\" alt=\"$title\"><br>";
                echo "<br>";
            }
        }
    } else {
        echo "No data found!";
    }
}

curl_close($curl);

?>