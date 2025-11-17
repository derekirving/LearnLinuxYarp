# Learn Linux Yarp

Hosting Linux boxes behind the SBS proxy.

The Linux box in this documentation is based on Ubuntu-18.04.ova available from [this OneDrive link](https://strath-my.sharepoint.com/:u:/r/personal/derek_irving_strath_ac_uk/Documents/VirtualBox/Ubuntu-18.04.ova?csf=1&web=1&e=oPyASy). It' setup with Host-Only and NAT networking.

## APACHE

Setup Apache

```bash
sudo apt update
sudo apt install apache2

# use UFW (the uncomplicated firewall) to allow web traffic
sudo ufw allow 'Apache'

#check that Apache is running
sudo systemctl status apache2
```

From the VM's web browser, `http://localhost` should display the Apache welcome page.

From the host, use `http://your_server_ip`

### Adjust the ownership of the `html` folder

Because `/var/www/html` is owned by root and has restricted permissions by default, you will see it as read-only or not be able to create/delete files.

```bash
sudo chown -R $USER:$USER /var/www/html
```

To access the folder from the Files app, press `Ctrl+L` in the Files window to input a path directly: `/var/www/html`.

## YARP proxy

The proxy is setup in the usual way, in the example to proxy from the endpoint `/proxy/**` to `http://192.168.56.101`.


```json
{
  "ReverseProxy": {
    "Routes": {
      "route1": {
        "ClusterId": "cluster1",
        "AuthorizationPolicy": "proxyPolicy",
        "Match": {
          "Path": "/proxy/{**catch-all}"
        },
        "Transforms": [
          {
            "PathRemovePrefix": "/proxy"
          }
        ]
      }
    },
    "Clusters": {
      "cluster1": {
        "Destinations": {
          "destination1": {
            "Address": "http://192.168.56.101/"
          }
        }
      }
    }
  }
}
```

An Authorization Policy named "proxyPolicy", is configured:

```csharp
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("proxyPolicy", policy =>
        policy.RequireAuthenticatedUser());
```