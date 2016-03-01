<?php
// script to create an entry for a given playername in the database

// contains connection function
include 'connect.php';

// GET player name from arguments
$name = $_GET["name"];


// the query
$query = "INSERT INTO Player (Username,Wins,Losses,Kills,Deaths)
VALUES (\"" . $name . "\",0,0,0,0);";


// initialize connection to database
$connection = connectToDb();


// Perform Query
$result = mysql_query($query, $connection);

// done

?>