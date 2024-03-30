<?php
include("auth.php");
include("storage.php");

header("Content-Type: application/json; charset=utf-8");
if ($_POST) {
    if (!isset($_POST["action"])) {
        http_response_code(400);
        die();  
    }

    switch($_POST["action"]) {
        case "register":
            if (!isset($_POST["username"]) || 
                !isset($_POST["password"])) {
                    http_response_code(400);
                    die();
                }
            register($_POST["username"], $_POST["password"]);
            break;

        case "login":
            if (!isset($_POST["username"]) || 
                !isset($_POST["password"])) {
                    http_response_code(400);
                    die();
                }
            login($_POST["username"], $_POST["password"]);
            break;

        case "save":
            if (!isset($_POST["token"]) ||
                !isset($_POST["title"]) ||
                !isset($_POST["content"])) {
                    http_response_code(400);
                    die();
                }
            save($_POST["token"], $_POST["title"], $_POST["content"]);
            break;

        case "titles":
            if (!isset($_POST["token"])) {
                    http_response_code(400);
                    die();
            }
            titles($_POST["token"]);
            break;

        case "content":
            if (!isset($_POST["token"]) ||
                !isset($_POST["title"])) {
                http_response_code(400);
                die();
            }
            content($_POST["token"], $_POST["title"]);
            break;
    }    
}
?>