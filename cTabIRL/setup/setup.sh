#!/bin/sh

# Basic script to install / update a ctab instance on a Linux server
# Requires dotnet SDK, see https://learn.microsoft.com/en-us/dotnet/core/install/linux, `sudo apt-get install -y dotnet-sdk-8.0` on Ubuntu 24.04 LTS

# Logs         : journalctl -fu kestrel-ctab -n 100
# Manual Stop  : sudo systemctl stop kestrel-ctab
# Manual Start : sudo systemctl start kestrel-ctab

if [ ! -d ~/build/ctab ]; then
	mkdir ~/build
	cd ~/build
	git clone https://github.com/jetelain/ctab.git ctab
fi

if [ ! -d /opt/ctab ]; then
	sudo mkdir /opt/ctab
	sudo chown $USER:$USER /opt/ctab
fi

if [ ! -d /var/www/ctab ]; then
	sudo mkdir /var/www/ctab
	sudo chown www-data:www-data /var/www/ctab
fi

if [ ! -d /var/www/ctab/images ]; then
	sudo mkdir /var/www/ctab/images
	sudo chown www-data:www-data /var/www/ctab/images
fi

if [ ! -d /var/www/aspnet-keys ]; then
	sudo mkdir /var/www/aspnet-keys
	sudo chown www-data:www-data /var/www/aspnet-keys
fi

cd ~/build/ctab

echo "Update git"
git checkout main
git pull
git submodule update --init --recursive --rebase --force

echo "Check config"
if [ ! -f /opt/ctab/appsettings.Production.json ]; then
	echo " * Create appsettings.Production.json"
	cp cTabIRL/setup/appsettings.Production.json /opt/ctab/appsettings.Production.json
	read -p "Type the Steam Api Key obtained from https://steamcommunity.com/dev/apikey, then press [ENTER]:" STEAM_API_KEY
	sed -i "s/STEAM_API_KEY/$STEAM_API_KEY/g"  /opt/ctab/appsettings.Production.json
fi

if [ ! -f /etc/systemd/system/kestrel-ctab.service ]; then
	echo " * Create kestrel-ctab.service"
	sudo cp cTabIRL/setup/kestrel-ctab.service /etc/systemd/system/kestrel-ctab.service
	
	sudo systemctl enable kestrel-ctab
fi

echo "Build"
rm -rf dotnet-webapp
dotnet publish -c ReleaseCloud -o dotnet-webapp -r linux-x64 --self-contained false cTabIRL/cTabWebApp/cTabWebApp.csproj

echo "Stop Service"
sudo systemctl stop kestrel-ctab

echo "Copy files"
cp -ar "dotnet-webapp/." "/opt/ctab"

echo "Start Service"
sudo systemctl start kestrel-ctab
