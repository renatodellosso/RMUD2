cd server
dotnet build

pid=$(sudo lsof -t -i:2001)
echo "Killing process $pid..."
sudo kill $pid

while true;
do
    sudo service apache2 restart
    dotnet run
done