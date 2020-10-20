
## Prepare

In order to install cTabWebApp on your communauty server, you will require :
- Linux server with Ubuntu 18+ (can work with other distributions, but commands to install will be different)
  - 2 GHz CPU or more
  - 1 GB RAM or more
  - 1 GB disk or more
  - Low latency network
- SSH client such as PuTTY
- SFTP client such as FileZilla
- User account on server with "sudo" rights and SSH access

If you have a large communauty, you can host up to 500 players, but server must have much more higher requirements :
- 4 GB or more RAM
- 8 CPU Cores at 2 Ghz or more (a single player requires at average 0.015 Cores at 2 GHz)
- 150 MB/s network (a single player requires at average 0.3 MB/s)
- Ability to handle simultaneous 1200 TCP open connections (a single player requires at average 2.1 TCP open connections)
- No other service hosted on server

## Install and configure cTabWebApp

### Install files

Create a directory for cTabWebApp files with command `sudo mkdir /var/www/ctab`

Ensure that you will be able to upload files with `sudo chown $USER:$USER /var/www/ctab`

Upload files to `/var/www/ctab` with a SFTP client

Make the server executable with `sudo chmod +x /var/www/ctab/cTabWebApp`

### Configure

Create a directory to host aspnet core keys with `sudo mkdir /var/aspnet-keys`

Ensure that server will be able to access that directory with `sudo chown www-data:www-data /var/aspnet-keys`

Edit cTabWebApp settings `nano /var/www/ctab/appsettings.json`

```json
{
  /* ... */
  "UnixKeysDirectory": "/var/aspnet-keys",
  // (Optional) "SteamKey": "XXXXXXXXXXXX", // Steam API key, to get from https://steamcommunity.com/dev/apikey
  "Communauty": {
    "Name": "Your communauty name here",
    "Home": "http:/yourhostname.com",
    "Contact": "https://discord.gg/yourDiscordHere or contact form (a way to contact you, legal obligation)",
    "PublicationDirector": "Your name or pseudonym (legal obligation)",
    "DPO": "Your name or pseudonym (legal obligation)",
    "Host": "Address and phone number of your Linux Server host company (legal obligation)"
  }
}
```

### Create service

Create a new systemd service, with command `sudo nano /etc/systemd/system/kestrel-ctab.service`

Copy-paste the following configuration:
```
[Unit]
Description=cTabWebApp

[Service]
WorkingDirectory=/var/www/ctab
ExecStart=/var/www/ctab/cTabWebApp --urls http://localhost:5010/
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=ctab
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

## Configure Apache2

Ensure that Apacche2 is installed with `sudo apt-get install apache2`

Create a new Apache2 web site configuration, with `sudo nano /etc/apache2/sites-available/001-ctab.conf`

Copy-paste the following configuration:
```
# This part might be already in 000-default.conf
<VirtualHost *:*>
	RequestHeader set "X-Forwarded-Proto" expr=%{REQUEST_SCHEME}
</VirtualHost>

# cTabWebApp
<VirtualHost *:80>
	ProxyPreserveHost On
	ProxyPass /ERROR/ !
	ProxyPass / http://localhost:5010/
	ProxyPassReverse / http://localhost:5010/
	# Here change the server name to the appropriate value !
	ServerName ctab.yourhostname.com
	ErrorLog ${APACHE_LOG_DIR}/ctab-error.log
	CustomLog ${APACHE_LOG_DIR}/ctab-access.log common
	Alias /ERROR/ /var/www/html/
	ErrorDocument 503 /ERROR/503.html
	RewriteEngine On
	RewriteCond %{HTTP:Upgrade} =websocket [NC]
	RewriteRule /(.*)           ws://localhost:5010/$1 [P,L]
</VirtualHost>
```

Enable required apach2 mods with `sudo a2enmod proxy proxy_http rewrite headers deflate proxy_wstunnel`

Then enable the web site with `sudo a2ensite 001-ctab`

Restart Apache2 to make configuration effective with `sudo systemctl reload apache2`

## Start and test web site

Start the cTabWebApp with `sudo systemctl start kestrel-ctab.service`

Open a web browser on the address you have configured (http://ctab.yourhostname.com for the previous example), you should be able to the see the web site.

In case of trouble, you can check service status with `sudo systemctl status kestrel-ctab.service`, or see logs with `journalctl -fu kestrel-ctab.service --since today`.

## Configure cTab IRL in-game

You may preconfigure cTab IRL server for all players on your Arma3 server, or let each user opt-in for your server.

Server address is configured with CBA Settings, in the cTab section (parameter name `ctab_irl_connect_uri`), address must ends with "/hub" (http://ctab.yourhostname.com/hub for the previous example)
