const deleteElement = (elementChild, parent) => {
  if (parent.childs) {
    const childs = parent.childs;
    while (childs.indexOf(elementChild) !== -1) {
      childs.splice(childs.indexOf(elementChild), 1);
    }
  }
};

const up = (elementChild, parent) => {
  const childs = parent.childs;
  const index = childs.indexOf(elementChild);
  childs.splice(index, 1);
  childs.splice(index - 1, 0, elementChild);
};

const down = (elementChild, parent) => {
  const childs = parent.childs;
  const index = childs.indexOf(elementChild);
  childs.splice(index, 1);
  childs.splice(index + 1, 0, elementChild);
};

const canUp = (elementChild, parent) => parent.childs.indexOf(elementChild) > 0;

const canDown = function(elementChild, parent) {
  return parent.childs.indexOf(elementChild) < parent.childs.length - 1;
};

export const menu = {
  up: up,
  down: down,
  canUp: canUp,
  canDown: canDown,
  deleteElement: deleteElement,
};
