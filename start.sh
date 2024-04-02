cd server
dotnet build

pid=$(sudo lsof -t -i:2001)
echo "Killing process $pid..."
sudo kill $pid

sudo service apache2 restart
dotnet run
