echo "Pulling from git..."
git pull
echo "Restarting Apache..."
sudo service apache2 restart
cd server/bin/Debug/net7.0
echo "Starting server..."
sudo dotnet server.dll