cd server
dotnet build

pid=$(sudo lsof -t -i:443)
echo "Killing process $pid..."
sudo kill $pid
TIMEOUT_SECS=10
timeout $TIMEOUT_SECS tail --pid=$pid -f /dev/null

sudo service apache2 restart
dotnet run
