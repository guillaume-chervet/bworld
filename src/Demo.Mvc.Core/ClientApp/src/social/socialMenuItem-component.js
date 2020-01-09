import React from 'react';

const SocialButton = ({ socialClassName, menuItem }) => {
  const classNameA = 'btn btn-block btn-social btn-' + socialClassName;
  const classNameSpan = 'fa fa-' + socialClassName;
  return (
    <a className={classNameA} target="_blank" href={menuItem.routePath}>
      <span className={classNameSpan} aria-hidden="true" />
      <span>{menuItem.title}</span>
    </a>
  );
};

export default class SocialMenuItem extends React.Component {
  constructor(props) {
    super(props);
  }
  render() {
    const className = this.props.className ? this.props.className : 'mw-social';
    const menuItem = this.props.menuItem;
    let Button = null;
    switch (menuItem.action) {
      case 'Email':
        Button = (
          <a className="btn btn-block btn-default" href={menuItem.routePath}>
            <span className="fa fa-envelope-o" aria-hidden="true" />
            <span>{menuItem.title}</span>
          </a>
        );
        break;
      case 'Phone':
        Button = (
          <a className="btn btn-block btn-default" href={menuItem.routePath}>
            <span className="fa fa-phone" aria-hidden="true" />
            <span>{menuItem.title}</span>
          </a>
        );
        break;
      case 'Other':
        Button = (
          <a
            className="btn btn-block btn-default mw-social--other"
            target="_blank"
            href={menuItem.routePath}>
            <span className="fa fa-globe" aria-hidden="true" />
            <span>{menuItem.title}</span>
          </a>
        );
        break;
      default:
        Button = (
          <SocialButton
            socialClassName={menuItem.action.toLowerCase()}
            menuItem={menuItem}
          />
        );
        break;
    }
    return <div className={className}> {Button} </div>;
  }
}
