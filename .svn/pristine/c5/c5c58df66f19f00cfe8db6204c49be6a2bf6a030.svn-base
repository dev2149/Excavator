<?php

$dbName = 'Database';
$host = 'Host';
$username = 'Username';
$password = 'Password';

$secretGameKey = "12345";
$secretServerKey = "54321";

function dbConnect()
{
	global  $dbName;
	global  $host;
	global  $username;
	global  $password;

	$link = mysql_connect($host, $username, $password);
	
	if(!$link)
	{
		fail("Couldn�t connect to database server");
	}
	
	if(!@mysql_select_db($dbName))
	{
		fail("Couldn�t find database $dbName");
	}
	
	return $link;
}
	
function safe($variable)
{
	$variable = addslashes(trim($variable));
	return $variable;
}

function fail($errorMsg)
{
	print $errorMsg;
	exit;
}

?>