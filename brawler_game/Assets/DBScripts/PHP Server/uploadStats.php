<?php
// upload a player's stats to the database

// contains connection function
include 'connect.php';

// initialize connection to database
$connection = connectToDb();

// GET parameters
$pid = $_GET["pid"];
$kills = $_GET["kills"];
$deaths = $_GET["deaths"];
$won = $_GET["win"];

$wins = 1;
$losses = 1;

// set won flag
if ($won == 1) {
	$losses = 0;
} else {
	$wins = 0;
}
// formulate query
$query = "UPDATE Player SET Kills=Kills + " . $kills .
		", Deaths=Deaths + " . $deaths .
		", Wins=Wins + " . $wins .
		", Losses=Losses + " . $losses .
		" WHERE Username=\"" . $pid . "\";";
				

// Perform Query
$result = mysql_query($query, $connection);
// Check result
// This shows the actual query sent to MySQL, and the error. Useful for debugging.
if (!$result) {
    echo "DB Error, could not query the database\n";
    echo 'MySQL Error: ' . mysql_error();
    exit;
}
?>