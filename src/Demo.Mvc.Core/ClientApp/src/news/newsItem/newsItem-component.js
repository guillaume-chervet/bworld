import app from '../../app.module';
import history from '../../history';
import { page } from '../../shared/services/page-factory';
import { newsItem } from './newsItem-factory';
import React, { useEffect, useMemo }from 'react';
import { PageBreadcrumbWithState } from '../../breadcrumb/pageBreadcrumb-component';
import { Div } from '../../elements/div/elementDiv-component';
import { Bottom } from '../../free/bottom/mwBottom-component';
import { react2angular } from 'react2angular';


const NewsItem = () => {

  const memData = useMemo(()=>{
    const _data = newsItem.data;
   const vm = {};
    const parentsJson = newsItem.mapParent({
      type: 'div',
      childs: _data.elements,
    });
    vm.element = parentsJson;
    const metaParentsJson = newsItem.mapParent({
      type: 'div',
      childs: _data.metaElements,
    });
    vm.metaElement = metaParentsJson;
    vm.url = history.url();
    vm.data = {
      userInfo: _data.userInfo,
      lastUpdateUserInfo: _data.lastUpdateUserInfo,
      isDisplayAuthor: _data.data.isDisplayAuthor,
      isDisplaySocial: _data.data.isDisplaySocial,
      createDate: _data.createDate,
      updateDate: _data.updateDate,
      urlNews: _data.urlNews,
    };
    return vm;
  },[]);

  useEffect(() =>{
    const title = newsItem.getTitle(memData.elements);
    page.setTitle(title);

  });
  return (
      <PageBreadcrumbWithState>
        <Div element={memData.element} />
        <Bottom data={memData.data} />
      </PageBreadcrumbWithState>
  );
};

const name = "newsItem";


app.component(name, react2angular(NewsItem, []));

export default name;
