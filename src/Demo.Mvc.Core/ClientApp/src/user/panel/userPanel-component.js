import app from '../../app.module';
import { page } from '../../shared/services/page-factory';
import { manageUser } from '../info/manageUser-service';
import { userNotification } from '../info/userNotification-factory';
import { master } from '../../shared/providers/master-provider';
import React, {useEffect} from 'react';
import { react2angular } from 'react2angular';
import { PageBreadcrumbWithState } from '../../breadcrumb/pageBreadcrumb-component';

const name = 'userPanel';

const UserPanel = () => {

  useEffect(() =>{
    page.setTitle('Accueil', page.types.user);
  });
  
  const user = manageUser.user;
  const notification = {
    numberUnreadMessage: userNotification.data.numberUnreadMessage,
  };
  const getInternalPath = master.getInternalPath;
  const goCreateSite = function() {
    window.location =
        'https://www.bworld.fr/site/c176c987-59b2-4a59-93bd-a54d73e3dc2e/authentification?dm=false';
  };
  
  return (


      <PageBreadcrumbWithState>
          <div>
            <h1><span className="glyphicon glyphicon-user ng-scope" />Utilisateur: {user.userName}</h1>
            <p>Bienvenue sur votre page utilisateur.</p>
            <div className="row">
              <div className="col-sm-6">
                <div className="panel panel-default">
                  <div className="panel-heading">
                    <h3 className="panel-title">Vos services</h3>
                  </div>
                  <div className="panel-body">
                    <ul>
                      <li><a href={getInternalPath('/utilisateur/messages')}><span
                          className="glyphicon glyphicon-envelope" aria-hidden="true" /><span> Messages</span>
                        {notification.numberUnreadMessage > 0 ? <span className="badge" >{notification.numberUnreadMessage}</span>: null}</a></li>
                      <li><a href={getInternalPath('/utilisateur/compte')}><span
                          className="glyphicon glyphicon-cog" aria-hidden="true" /><span> Paramètres</span></a></li>
                    </ul>
                  </div>
                </div>
              </div>

              <div className="col-sm-6">
                <div className="panel panel-default">
                  <div className="panel-heading">
                    <h3 className="panel-title">Vos sites</h3>
                  </div>
                  <div className="panel-body">
                    {user.sites.length > 0 ? (<ul>
                      {user.sites.map(site => <li><a href={site.url} >{site.url}</a></li>)}
                        </ul>) :
                        (<div className="text-center" ng-if="$ctrl.user.sites.length <= 0">
                          <button type="button" className="btn btn-primary btn-lg"
                                  onClick={goCreateSite}>Démarrer
                            votre site
                          </button>
                        </div>)
                    }
                  </div>
                </div>
              </div>
              <div className="clearfix" />
            </div>
          </div>
        </PageBreadcrumbWithState>

  );
};

app.component(name, react2angular(UserPanel, []));

export default name;