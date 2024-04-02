pid=$(sudo lsof -t -i:443)
echo "Killing process $pid..."
sudo kill $pid
TIMEOUT_SECS=10
timeout $TIMEOUT_SECS tail --pid=$pid -f /dev/null
dotnet build
dotnet run