Bonjour §model.UserName§,<br />
<br />
	§if(model.IsReply)§
	Vous venez de reçevoir la réponse ci-dessous, depuis le site "§model.SiteName"<br />
	§else§
	Vous venez de reçevoir le message ci-dessous, depuis le site "§model.SiteName§"<br />
	Inscrivez-vous afin d'échanger plus rapidement et simplement. <br />
	§endif§
	<br />
	Titre: §model.Title§<br />
	§if(model.Sender.IsNotAuthenticated)§
	Nom: §model.Sender.FullName§<br />
	Email: §model.Sender.Email§<br />
	Téléphone: §model.Sender.Phone§<br />
	§endif§
	Message: <br />
	§model.Message§<br />
	<br />
	§if(model.MessageUrl)§
	<a href="§model.MessageUrl§">Voire votre message</a><br /><br />
	§endif§
Cordialement,<br />
Service Client §model.SiteName§,<br />
<a href="§model.SiteUrl§">§model.SiteUrl§</a>