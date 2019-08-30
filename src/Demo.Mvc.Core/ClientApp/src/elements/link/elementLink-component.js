import app from '../../app.module';
import { service as linkService } from './elementLink-factory';
import { audit } from '../../shared/services/audit-factory';

import React from 'react';
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';

const getUrl = data => {
  switch (data.type) {
    case 'phone':
      return `tel: ${data.url}`;
    case 'mail':
      return `mailto:${data.url}`;
    case 'facebook':
      return data.url;
    default:
      return linkService.getPath(data);
  }
};

export const Link =({element}) => {
  const onClick = () => {
    const data = element.data;
    const href = getUrl(data);
    audit.trace(href, 'Button');
  };
    const data = element.data;
    const title = linkService.getTitle(data, data.label);
    const href = getUrl(data);

    switch (data.type) {
      case 'phone':
        return (
          <div className="text-center mw-link">
            <a
              className="btn btn-lg btn-success"
              href={href}
              onClick={onClick}>
              <span className="fa fa-phone" />
              <span> {title}</span>
            </a>
          </div>
        );
      case 'mail':
        return (
          <div className="text-center mw-link">
            <a
              className="btn btn-lg btn-success"
              href={href}
              onClick={onClick}>
              <span className="fa fa-envelope-o" />
              <span> {title}</span>
            </a>
          </div>
        );
      case 'facebook':
        return (
          <div className="text-center mw-link">
            <a
              className="btn btn-social btn-facebook"
              href={href}
              onClick={onClick}>
              <span className="fa fa-facebook" />
              <span> {title}</span>
            </a>
          </div>
        );
      default:
        return (
          <div className="text-center mw-link">
            <a
              href={href}
              onClick={onClick}
              className="btn btn-primary btn-lg mw-btn-link">
              {' '}
              {title}
            </a>
          </div>
        );
    }
};

const name = 'elementLink';
app.component(name, react2angular(Link, ['element']));

export default name;
