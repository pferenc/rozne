Shoper C# 

Shoper Shop new Shoper("login", "passs");

Shop.ShopUrl = "http://myshoponshoper.pl"

Object[] details = { Shop.Session, "attribute.list", new Object[] { true, null } };

var detailsJson = Shop.SendApiRequest("call", details);

-----------------------------------------------------------------------------
