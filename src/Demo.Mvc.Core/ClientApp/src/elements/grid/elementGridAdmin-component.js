import React from "react";
import {ElementAdmin} from "../elementAdmin-component";
import {Grid} from "./elementGrid-component";
import {ElementGridElementAdmin} from "../gridElement/elementGridElementAdmin-component";

const GridEdit = ({element, onChange}) => {
  return <>  {element.childs.map((childElement, $index) => (
      <>
        <ElementGridElementAdmin element={childElement} onChange={onChange} />
        {($index + 1) % 2 === 0 && (
            <div className="clearfix visible-sm-block"></div>
        )}
        {($index + 1) % 3 === 0 && (
            <div className="clearfix visible-md-block"></div>
        )}
        {($index + 1) % 4 === 0 && (
            <div className="clearfix visible-lg-block"></div>
        )}
      </>
  ))}
    <div className="clearfix"/>
  </>;
  
};

export const ElementGridAdmin = ({ element, mode, onChange }) => {
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Grille'}
          adminEdit={<GridEdit element={element} onChange={onChange} />}
          adminView={<Grid element={element} />}
      >
      </ElementAdmin>
  );
};

