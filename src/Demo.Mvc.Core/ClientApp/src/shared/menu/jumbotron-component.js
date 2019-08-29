import app from '../../app.module';
import { connect } from 'react-redux'
import {react2angular} from "react2angular";
import {withStore} from "../../reducers.config";
import LoadableCarousel from "../../elements/carousel/LoadableCarousel";
import React from "react";

const name = 'jumbotron';

const CarouselJumbotron = ({master}) => {
    return (master.imageLogos.length > 1 && (
        <LoadableCarousel infiniteLoop={true} showThumbs={false} showIndicators={false} autoPlay={true} showArrows={false}>
            {master.imageLogos.map(slide => (<div key={slide.url} >
                <img src={slide.url}/>
                {slide.description && (<p className="legend">Legend 1</p>)}
            </div>))}
        </LoadableCarousel>));
};

const Jumbrotron = ({master}) => {
    return (master.isLogo && (<div className="ng-cloak mw-jumbotron">

        {!master.isJumbotron && (<div>
            {master.imageLogos.length === 1 && (<img src={master.logoUrl}
                 className="center-block img-responsive" alt="Logo du site"/>)}

        <CarouselJumbotron master={master}></CarouselJumbotron>
        </div>)}

        {master.isJumbotron && (<div  className="jumbotron">
            <div className="container">
                <img ng-if="$ctrl.master.imageLogos.length === 1" ng-src="{{$ctrl.master.logoUrl}}"
                     className="center-block" alt="Logo du site"/>
               <CarouselJumbotron master={master}></CarouselJumbotron>
            </div>
        </div>)}
    </div>));
};



const mapStateToProps = (state, ownProps) => {
    return { master: state.master.masterData };
};

const mapDispatchToProps = (dispatch, ownProps) => {
    return {};
};

const JumbotronWithState = withStore(connect(
    mapStateToProps,
    mapDispatchToProps
)(Jumbrotron));

app.component(name, react2angular(JumbotronWithState, ['master']));

export default name;