import app from '../../app.module';
import history from '../../history';
import { Empty } from '../../shared/directives/empty-component';
import { page } from '../../shared/services/page-factory';
import { news } from './news-factory';
import { dialogTags } from '../../admin/tags/dialogTags-factory';
import { isDraft, isDeleted } from '../../shared/itemStates';
import React, { useEffect, useMemo, useState } from 'react';
import Tags from '../tags-component';
import { User } from '../../free/user/mwUser-component';

import './news.css';
import { Div } from '../../elements/div/elementDiv-component';
import { PageBreadcrumbWithState } from '../../breadcrumb/pageBreadcrumb-component';
import { react2angular } from 'react2angular';

const name = 'news';

const SwitchGallerie = ({ items, vm }) => (
  <div>
    {items.map(item => (
      <div key={item.data.viewUrl}>
        <div className="col-sm-6 col-md-6 col-lg-4">
          <div className="panel panel-default">
            <div className="panel-heading">
              <h3 className="panel-title">
                <a className="mw-title" href={item.data.viewUrl}>
                  {item.data.title}
                </a>
                {vm.isDraft(item.data) ? (
                  <span className="label label-default">Brouillon</span>
                ) : null}
                {vm.isDeleted(item.data) ? (
                  <span className="label label-danger">Supprimé</span>
                ) : null}
                <Tags tags={item.data.tags} />
              </h3>
            </div>
            <div>
              <a
                href={item.data.viewUrl}
                title={item.data.title}
                className="hand">
                <img
                  src={vm.getFirstImage(item.element).thumbnailUrl}
                  className="center-block img-responsive img-thumbnail"
                  alt={vm.getFirstImage(item.element).name}
                />
              </a>
            </div>
          </div>
        </div>
        {(items.indexOf(item) + 1) % 2 === 0 ? (
          <div className="clearfix visible-sm-block"></div>
        ) : null}
        {(items.indexOf(item) + 1) % 2 === 0 ? (
          <div className="clearfix visible-md-block"></div>
        ) : null}
        {(items.indexOf(item) + 1) % 3 === 0 ? (
          <div className="clearfix visible-lg-block"></div>
        ) : null}
      </div>
    ))}

    <div className="clearfix"></div>
  </div>
);

const SwitchDefault = ({ items, vm }) => (
  <div>
    {items.map(item => (
      <div className="panel panel-default" key={item.data.viewUrl}>
        <div className="panel-heading">
          <h3 className="panel-title">
            <a className="mw-title" href={item.data.viewUrl}>
              {item.data.title}
            </a>
            {vm.isDraft(item.data) ? (
              <span className="label label-default">Brouillon</span>
            ) : null}
            {vm.isDeleted(item.data) ? (
              <span className="label label-danger">Supprimé</span>
            ) : null}
            <Tags tags={item.data.tags} />
          </h3>
        </div>
        <div className="panel-body">
          <Div element={item.element} />
          {item.data.hasNext ? (
            <a
              className="pull-right btn btn-default btn-lg"
              href={item.data.viewUrl}>
              Lire la suite
            </a>
          ) : null}
        </div>
        <div className="panel-footer">
          <User data={item.data}></User>
        </div>
      </div>
    ))}
  </div>
);

const News = ({ vm, onTagChange }) => {
  const Content =
    vm.data.getDisplayMode() === 'galerie' ? SwitchGallerie : SwitchDefault;
  return (
    <PageBreadcrumbWithState className="mw-news">
      <Div element={vm.element} />
      <hr />
      <div className="row">
        <div className="col-sm-9">
          <Empty items={vm.items} content={'Aucun article.'} />
          <Content items={vm.items} vm={vm} />
          {vm.data.hasPreviousOrNext() ? (
            <nav>
              <ul className="pager">
                {vm.data.hasPrevious() ? (
                  <li className="previous">
                    <a href={vm.data.urlPrevious}>
                      <span aria-hidden="true">&larr;</span> Précédent
                    </a>
                  </li>
                ) : null}
                {vm.data.hasNext() ? (
                  <li className="next">
                    <a href={vm.data.urlNext}>
                      Suivant<span aria-hidden="true">&rarr;</span>
                    </a>
                  </li>
                ) : null}
              </ul>
            </nav>
          ) : null}
        </div>
        <div className="col-sm-3">
          <div className="panel panel-default">
            <div className="panel-heading">
              <h3 className="panel-title">Filtres</h3>
            </div>
            <div className="panel-body">
              <div className="form-group" nf-if="vm.tags.length>0">
                <label className="control-label">Tags: </label>
                {vm.tags.map(tag => (
                  <div key={tag.id} className="checkbox">
                    <label>
                      <input
                        value={tag.ticked}
                        type="checkbox"
                        onChange={() => onTagChange(tag)}
                      />{' '}
                      {tag.name}
                    </label>
                  </div>
                ))}
              </div>
              <button
                className="btn btn-lg btn-default pull-right"
                type="button"
                onClick={vm.filter}>
                {' '}
                Filtrer
              </button>
            </div>
          </div>
        </div>
      </div>
    </PageBreadcrumbWithState>
  );
};

const NewsContainer = () => {
  const memData = useMemo(() => {
    const vm = {};
    vm.isDeleted = isDeleted;
    vm.isDraft = isDraft;

    const items = news.data.items.reduce((accumulator, item) => {
      if (!isDraft(item)) {
        accumulator.push(item);
      }
      return accumulator;
    }, []);

    vm.items = items;
    vm.getFirstImage = news.getFirstImage;

    const parentsJson = news.mapParent({
      type: 'div',
      childs: news.data.elements,
    });
    vm.element = parentsJson;
    const metaParentsJson = news.mapParent({
      type: 'div',
      childs: news.data.metaElements,
    });
    vm.metaElement = metaParentsJson;
    vm.data = {
      userInfo: news.data.userInfo,
      lastUpdateUserInfo: news.data.lastUpdateUserInfo,
      createDate: news.data.createDate,
      updateDate: news.data.updateDate,
      hasPrevious: () => {
        return news.data.hasPrevious;
      },
      urlPrevious: news.data.urlPrevious,
      hasNext: () => {
        return news.data.hasNext;
      },
      urlNext: news.data.urlNext,
      hasPreviousOrNext: () => {
        return news.data.hasPrevious || news.data.hasNext;
      },
      getDisplayMode: () => {
        return news.data.displayMode;
      },
      getNumberItemPerPage: () => {
        return news.data.numberItemPerPage;
      },
    };

    vm.filter = () => {
      const tags = [];
      vm.tags.map(tag => {
        if (tag.ticked) {
          tags.push(tag.id);
        }
      }, this);

      history.search({ index: null, tags: tags });
    };
    return vm;
  }, []);

  useEffect(() => {
    const title = news.getTitle(memData.element.childs);
    page.setTitle(title);
  });

  const selectedTags = () => {
    const search = history.search();
    const selectedTags = [];
    const tags = search.tags;
    if (tags) {
      if (Array.isArray(tags)) {
        tags.forEach(tag => selectedTags.push(tag));
      } else {
        selectedTags.push(tags);
      }
    }
    return selectedTags;
  };

  const initialTagState = dialogTags.initTags(
    dialogTags.model.items.tags,
    [],
    selectedTags()
  );

  const [tags, setState] = useState(initialTagState);

  const onTagChange = tag => {
    const newTags = tags.map(t => {
      if (t.id === tag.id) {
        return { ...tag, ticked: !tag.ticked };
      }
      return tag;
    });
    setState(newTags);
  };

  const data = { ...memData, tags };

  return <News vm={data} onTagChange={onTagChange} />;
};

app.component(name, react2angular(NewsContainer, []));

export default name;
