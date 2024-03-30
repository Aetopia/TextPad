<?php
$mysqli = new mysqli("localhost", "root", "");
$mysqli->query("drop database if exists textpad_users");
$mysqli->query("drop database if exists textpad_users_data");
$mysqli->query("create database if not exists textpad_users");
$mysqli->query("create database if not exists textpad_users_data");
$mysqli->query("use textpad_users");
$mysqli->query("create table if not exists users(
                username varchar(16) UNIQUE, 
                password varchar(60), 
                token char(32))");
$mysqli->commit();
$mysqli->close();
?>