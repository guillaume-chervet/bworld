import app from '../app.module';
import { page } from '../shared/services/page-factory';
import { free } from './free-factory';
import React, { useEffect, useMemo }from 'react';
import { PageBreadcrumbWithState } from '../breadcrumb/pageBreadcrumb-component';
import { Div } from '../elements/div/elementDiv-component';
import { Bottom } from './bottom/mwBottom-component';
import { react2angular } from 'react2angular';

const name = "free";

const Free = () => {

  const memData = useMemo(()=>{
    const _data = free.data;
    const parentsJson = free.mapParent({
      type: 'div',
      childs: _data.elements,
    });
    const data = {};
    data.element = parentsJson;
    const metaParentsJson = free.mapParent({
      type: 'div',
      childs: _data.metaElements,
    });
    data.metaElement = metaParentsJson;
    data.data = {
      userInfo: _data.userInfo,
      lastUpdateUserInfo: _data.lastUpdateUserInfo,
      isDisplayAuthor: _data.data.isDisplayAuthor,
      isDisplaySocial: _data.data.isDisplaySocial,
      createDate: _data.createDate,
      updateDate: _data.updateDate,
    };
  return data;
  },[]);
  
  useEffect(() =>{
    const title = free.getTitle(memData.elements);
    page.setTitle(title);
  });
  return (
      <PageBreadcrumbWithState>
        <Div className="mw-free" element={memData.element} />
        <Bottom data={memData.data} />
      </PageBreadcrumbWithState>
  );
};

app.component(name, react2angular(Free, []));

export default name;
