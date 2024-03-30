<?php
declare(strict_types=1);

function check_username_password(string $username, string $password) {
    $username_length = strlen($username = trim($username));
    $password_length = strlen($password);

    if (!($username_length >= 4 && $username_length <= 16) || 
    !($password_length >= 8 && $password_length <= 16)) {
        http_response_code(400);
        die();    
    }

    if (str_contains($username, " ") ||
        ctype_digit($username[0]) || 
        !ctype_alnum($username) ||
        !ctype_alnum($password)) {
            http_response_code(400);
            die();     
        }
}

function register(string $username, string $password) {
    check_username_password($username = trim($username), $password);
    
    $mysqli = new mysqli("localhost", "root", "", "textpad_users");
    $password_hash = password_hash($password, PASSWORD_BCRYPT);

    if (!isset($mysqli->query("select username from users where username = '$username'")->fetch_all()[0])) {
        $mysqli->query("insert into users values('$username', '$password_hash', null)");
        $mysqli->query("use textpad_users_data");
        $mysqli->query("create table $username(title TINYTEXT UNIQUE, content longtext)");
    }
    else {
        http_response_code(403);
        die(); 
    }

    $mysqli->commit();
    $mysqli->close();

    http_response_code(201);
}

function login(string $username, string $password) {
    check_username_password($username = trim($username), $password);

    $mysqli = new mysqli("localhost", "root", "", "textpad_users");
    $row = @$mysqli->query("select * from users where username = '$username'")->fetch_assoc();
    
    if ($row == null) {
        http_response_code(404);
        die();
    }

    if ($row["username"] != $username ||
        !password_verify($password, $row["password"])) {
        http_response_code(404);
        die();
    }

    $token = bin2hex(random_bytes(16));
    $mysqli->query("update users set token = '$token' where username = '$username'");

    $mysqli->commit();
    $mysqli->close();

    echo $token; 
    http_response_code(200);
}
?>