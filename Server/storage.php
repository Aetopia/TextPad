<?php
declare(strict_types=1);

function save(string $token, string $title, string $content) {
    $mysqli = new mysqli("localhost", "root", "", "textpad_users");

    $token = trim($mysqli->real_escape_string($token));
    $title = trim($mysqli->real_escape_string($title));
    $title_length = strlen($title);
    $content = trim(str_replace("\r\n", "\n", $mysqli->real_escape_string($content)));

    $row = @$mysqli->query("select username from users where token = '$token'")->fetch_assoc();
    if ($row == null || 
        !($title_length  >= 1 && $title_length <= 255)) {
        http_response_code(400);
        die();
    }

    $mysqli->query("use textpad_users_data");
    try {$mysqli->query("insert into ".$row["username"]." values('$title', '$content')");}
    catch (Exception $e) {$mysqli->query("update ".$row["username"]." set content = '$content' where title = '$title'");}

    $mysqli->close();
    http_response_code(200);
}

function titles(string $token) {
    $mysqli = new mysqli("localhost", "root", "", "textpad_users");
    $token = trim($mysqli->real_escape_string($token));

    $row = @$mysqli->query("select username from users where token = '$token'")->fetch_assoc();
    if ($row == null) {
        http_response_code(400);
        die();
    }
    $username = $row["username"];
    
    $mysqli->query("use textpad_users_data");
    $titles = [];
    foreach($mysqli->query("select title from $username")->fetch_all() as $row) 
        array_push($titles, $row[0]);

    echo json_encode($titles);
    $mysqli->close();
    http_response_code(200);
}

function content(string $token, string $title) {
    $mysqli = new mysqli("localhost", "root", "", "textpad_users");
    $token = trim($mysqli->real_escape_string($token));
    $title = trim($mysqli->real_escape_string($title));

    $row = @$mysqli->query("select username from users where token = '$token'")->fetch_assoc();
    if ($row == null) {
        http_response_code(400);
        die();
    }
    $username = $row["username"];
    
    $mysqli->query("use textpad_users_data");
    $row = @$mysqli->query("select content from $username where title = '$title'")->fetch_assoc();
    if ($row == null) {
        http_response_code(400);
        die();
    }

    echo $row["content"];
    $mysqli->close();
    http_response_code(200);
}
?>