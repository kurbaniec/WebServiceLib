@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo Additional CURL Testing for Monster Trading Cards Game
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:8080/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:8080/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:8080/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:8080/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:8080/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:8080/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.
echo.

REM --------------------------------------------------
echo 3) create packages (done by "admin")
curl -X POST http://localhost:8080/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"space1\", \"Name\":\"SpaceMarine\", \"Damage\": 10.0}, {\"Id\":\"space2\", \"Name\":\"SpaceMarine\", \"Damage\": 12.0}, {\"Id\":\"space3\", \"Name\":\"SpaceMarine\", \"Damage\": 14.0}, {\"Id\":\"space4\", \"Name\":\"SpaceMarine\", \"Damage\": 16.0}, {\"Id\":\"space5\", \"Name\":\"SpaceMarine\",    \"Damage\": 18.0}]"
echo.																																																																																		 				    
curl -X POST http://localhost:8080/packages --header "Content-Type: application/json" --header "Authorization: Basic admin-mtcgToken" -d "[{\"Id\":\"ork1\", \"Name\":\"Ork\", \"Damage\": 10.0}, {\"Id\":\"ork2\", \"Name\":\"Ork\", \"Damage\": 12.0}, {\"Id\":\"ork3\", \"Name\":\"Ork\", \"Damage\": 14.0}, {\"Id\":\"ork4\", \"Name\":\"Ork\", \"Damage\": 16.0}, {\"Id\":\"ork5\", \"Name\":\"Ork\",    \"Damage\": 18.0}]"
echo.																																																																																		 				    
echo.

REM --------------------------------------------------
echo 4) acquire packages
curl -X POST http://localhost:8080/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:8080/transactions/packages --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d ""
echo.
echo.

REM --------------------------------------------------
echo 5) configure deck
curl -X PUT http://localhost:8080/deck --header "Content-Type: application/json" --header "Authorization: Basic kienboec-mtcgToken" -d "[\"space1\", \"space2\", \"space3\", \"space4\"]"
echo.
curl -X GET http://localhost:8080/deck --header "Authorization: Basic kienboec-mtcgToken"
echo.
curl -X PUT http://localhost:8080/deck --header "Content-Type: application/json" --header "Authorization: Basic altenhof-mtcgToken" -d "[\"ork1\", \"ork2\", \"ork3\", \"ork4\"]"
echo.
curl -X GET http://localhost:8080/deck --header "Authorization: Basic altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 6) battle
start /b "kienboec battle" curl -X POST http://localhost:8080/battles --header "Authorization: Basic kienboec-mtcgToken"
start /b "altenhof battle" curl -X POST http://localhost:8080/battles --header "Authorization: Basic altenhof-mtcgToken"
ping localhost -n 5 >NUL 2>NUL

REM --------------------------------------------------
echo 7) scoreboard
curl -X GET http://localhost:8080/score --header "Authorization: Basic kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 8) battle log
echo get battle history
curl -X GET http://localhost:8080/admin/battles/0  --header "Authorization: Basic admin-mtcgToken"
echo.
echo get specific batte log
curl -X GET http://localhost:8080/admin/battle/1  --header "Authorization: Basic admin-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo end...

REM this is approx a sleep 
ping localhost -n 100 >NUL 2>NUL
@echo on
