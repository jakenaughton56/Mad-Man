<?php
// fetch stats from the database and return them in JSON form

// contains connection function
include 'connect.php';

// initialize connection to database
$connection = connectToDb();

$query = "SELECT COUNT(*) FROM Player";

// Perform Query
$result = mysql_query($query, $connection);

$numPlayers = mysql_fetch_array($result);

$numPlayers = $numPlayers[0];


$query = "SELECT * FROM Player;";
				

// Perform Query
$result = mysql_query($query, $connection);

// Check result
// This shows the actual query sent to MySQL, and the error. Useful for debugging.
if (!$result) {
    echo "DB Error, could not query the database\n";
    echo 'MySQL Error: ' . mysql_error();
    exit;
}

$rows = Array();


$rows["numPlayers"] = $numPlayers;
// fetch all rows from the result
while($row = mysql_fetch_assoc($result)){
	// add each row to an array
  array_push($rows, $row);
}
// json encode the row and then send to webpage
echo json_encode($rows);
?>