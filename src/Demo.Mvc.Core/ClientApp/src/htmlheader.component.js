
import React from 'react';
import {Helmet} from "react-helmet";
import ReactDOM from 'react-dom';
import { react2angular } from 'react2angular';
import app from './app.module';

const Htmlheader = (props) => {
    
    const header = window.params.header;
        return (
            <>
                <Helmet>
                    <title>{header.title}</title>
                    <meta name="description" mw-meta="master.metaDescription" content={header.metaDescription}/>
                    <meta name="keywords" mw-meta="master.metaKeyword" content={header.metaKeywords}/>
                
                    <link rel="alternate" href={header.fullUrl} hrefLang="fr-fr"/>
                    <link rel="canonical" href={header.baseUrl}/>
                    <link rel="icon" type="image/png" href={header.iconeUrl}/>
                </Helmet>
            </>
        );
}

const name = 'htmlheader';
app.component(name, react2angular(Htmlheader, ['header']));

export default name;
        