import app from '../../app.module';
import {ElementAdmin} from "../elementAdmin-component";
import {react2angular} from "react2angular";
import React from "react";
import {MessageDirtyContainer} from "./elementMessageContainer";

const name = 'elementMessageAdmin';

const MessageAdmin = ({element, onAdd, onDelete, onChangeTitle}) => {
  return (<table className="table table-striped">
    <thead>
    <tr>
      <th>Titre messages</th>
      <th>Effacer</th>
    </tr>
    </thead>
    <tbody>
    {element.data.subjects.map((subject, index) => <tr>
      <td><input id="Title" type="text" name="Title" value={subject.title} onChange={(e)=>onChangeTitle(e, index)} className="form-control"/></td>
      <td>
        <button type="button" className="btn btn-danger" onClick={() => onDelete(index)}><span
    className="glyphicon glyphicon-remove"/></button>
      </td>
    </tr>)}
    <tr>
      <td colSpan="1"/>
      <td>
        <button type="button" className="btn btn-default" onClick={onAdd}><span
    className="glyphicon glyphicon-plus"/></button>
      </td>
    </tr>
    </tbody>
  </table>)
}

export const ElementMessageAdmin = ({ element, mode, onChange }) => {
  element = {...element};
  if(element.data) {
    if(Array.isArray(element.data)){
      element.data = {subjects:[...element.data]}
    }
  } 

  const onAdd = () => {
    let data;
    if(element.data && element.data.subjects){
        data = {subjects:[...element.data.subjects]};
    } else{
      data = {subjects:[]};
    }
    data.subjects.push({
      title: '',
    });
    onChange({ what: "element-edit", element: {...element, data: data}});
  };

  const onDelete = (subjectIndex) => {
    const subjects = [...element.data.subjects];
    subjects.splice(subjects.indexOf(subjectIndex), 1);
    onChange({ what: "element-edit", element: {...element, data: {subjects}}});
  };
  
  const onChangeTitle= (e, subjectIndex) =>{
    const subjects = [...element.data.subjects];
    subjects[subjectIndex] = {...subjects[subjectIndex], title : e.target.value }
    onChange({ what: "element-edit", element: {...element, data: {subjects}}});
  }
  
  return (
      <ElementAdmin
          onChange={onChange}
          element={element}
          adminTitle={'Formulaire envoi message'}
          adminEdit={<MessageAdmin
              element={element}
              onDelete={onDelete}
              onAdd={onAdd}
              onChangeTitle={onChangeTitle}
          />}
          adminView={<MessageDirtyContainer element={element} />}>
      </ElementAdmin>
  );
};

app.component(name, react2angular(ElementMessageAdmin, ['element', 'mode', 'onChange']));

export default name;
