import app from '../app.module';
import { page } from '../shared/services/page-factory';
import React, { useEffect } from 'react';
import { react2angular } from 'react2angular';

const name = 'default';

export const Default = () => {
  useEffect(() => {
    page.setTitle('Pas de page configurée');
  });

  return (
    <div className="row">
      <div className="col-md-10 col-md-offset-1 col-xs-12 col-xs-offset-0">
        <h1>Aucune page n'est configurée</h1>
        <p>
          Ceci est la page par défaut. Elle est visible car aucune page n'a été
          configurée pour ce site.
        </p>
      </div>
    </div>
  );
};

app.component(name, react2angular(Default, ['data']));

export default name;
