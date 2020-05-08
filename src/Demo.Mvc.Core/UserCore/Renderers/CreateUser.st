Bonjour §model.UserName§,<br />
<br />
	Vous venez de créer votre compte utilisateur bworld. Félicitation! <br />
§if(model.Provider)§
	Vous avez associé votre compte utilisateur à partir de votre login <b>§model.Provider§</b>.<br />
§endif§ 

	Vous devez confirmer votre email en cliquant sur le lien suivant: <br />
		<a href="§model.CallbackUrl§">Confimez votre email</a><br />
<br />
Cordialement,<br />
Guillaume Chervet,<br />
guillaume.chervet@bworld.fr<br />
<a href="https://www.bworld.fr">https://www.bworld.fr</a>