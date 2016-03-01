<?php
// create the table in the DB to store all the player stats

// contains connection function
include 'connect.php';

// the query
$query = "CREATE TABLE Player
(
Username VARCHAR(30),
PlayerID INT AUTO_INCREMENT,
Wins INT,
Losses INT,
Kills INT,
Deaths INT,
PRIMARY KEY (PlayerID)
);";


// initialize connection to database
$connection = connectToDb();


// Perform Query
$result = mysql_query($query, $connection);

// done

?>