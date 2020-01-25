import React from 'react';

const hashCode = str => {
  // java String#hashCode
  let hash = 0;
  for (let i = 0; i < str.length; i++) {
    hash = str.charCodeAt(i) + ((hash << 5) - hash);
  }
  return hash;
};

const intToRGB = i => {
  const c = (i & 0x00ffffff).toString(16).toUpperCase();
  return '00000'.substring(0, 6 - c.length) + c;
};

const stringToRGB = str => {
  return intToRGB(hashCode(str));
};

const Tags = ({ tags }) => {
  return tags.map(tag => (
    <span
      className="label"
      key={tag.id}
      style={{
        backgroundColor: `#${stringToRGB(tag.id)}`,
        marginRight: '2px',
        marginLeft: '2px',
      }}>
      {tag.name}{' '}
    </span>
  ));
};

export default Tags;
