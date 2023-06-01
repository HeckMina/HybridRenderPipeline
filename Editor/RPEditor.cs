using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Alice.Rendering.Hybrid;


namespace Alice.Rendering
{
    public class RPEditor : EditorWindow
    {
        public List<Node> mNodes=new List<Node>();
        public List<Link> mLinks = new List<Link>();
        Vector2 mLastMouseButtonDownPosition;
        Vector2 mDragPosition,mDragOffsetToLocalOriginalPosition;
        Node mLastMiddleButtonTouchedNode = null;
        Node mEditNameNode = null;
        Node mCurrentSelectedNode = null;
        RTSetting mRTSettingInfo = new RTSetting();
        RPSetting mRPSettingInfo = new RPSetting(0,0,300,400);
        LinkEditor mTempLink=null;
        bool mbShowCreateOptions = false;
        HybridPipelineData mCurrentEditPipelineData;
        public static RPEditor mInstance = null;
        public RPEditor()
        {
            mInstance = this;
        }
        public void SetPipelineData(Hybrid.HybridPipelineData inPipelineData){
            mCurrentEditPipelineData=inPipelineData;
            for(int i=0;i<mCurrentEditPipelineData.RenderPasses.Count;++i){
                RPNodeEditor node=new RPNodeEditor(mCurrentEditPipelineData.RenderPassRects[i]);
                node.mName=mCurrentEditPipelineData.RenderPasses[i];
                node.mID=mCurrentEditPipelineData.RenderPassIDs[i];
                node.mAttachedScriptName=mCurrentEditPipelineData.RenderPassScriptNames[i];
                node.mQueue=mCurrentEditPipelineData.RenderPassQueues[i];
                node.mLayerMask=mCurrentEditPipelineData.RenderPassLayerMasks[i];
                node.mEnterRenderPassAction=mCurrentEditPipelineData.RenderPassOnEnterActions[i];
                node.mColorRTLoadAction=mCurrentEditPipelineData.RenderPassColor0LoadActions[i];
                node.mColorRTStoreAction=mCurrentEditPipelineData.RenderPassColor0StoreActions[i];
                node.mDSRTLoadAction=mCurrentEditPipelineData.RenderPassDSLoadActions[i];
                node.mDSRTStoreAction=mCurrentEditPipelineData.RenderPassDSStoreActions[i];
                node.mOutputColor0=mCurrentEditPipelineData.RenderPassColor0Outputs[i];
                node.mOutputDS=mCurrentEditPipelineData.RenderPassDSOutputs[i];
                mNodes.Add(node);
            }
            for(int i=0;i<mCurrentEditPipelineData.RenderPassLightModes.Count;++i){
                string lightMode=mCurrentEditPipelineData.RenderPassLightModes[i];
                string[] splited=lightMode.Split('+');
                RPNodeEditor node=GetNode(splited[0]) as RPNodeEditor;
                node.mLightModes.Add(splited[1]);
            }
            for(int i=0;i<mCurrentEditPipelineData.RenderTargets.Count;++i){
                RTNodeEditor node=new RTNodeEditor(mCurrentEditPipelineData.RenderTargetRects[i]);
                node.mName=mCurrentEditPipelineData.RenderTargets[i];
                node.mID=mCurrentEditPipelineData.RenderTargetIDs[i];
                node.mFormat=mCurrentEditPipelineData.RenderTargetFormats[i];
                mNodes.Add(node);
            }
            for(int i=0;i<mCurrentEditPipelineData.Links.Count;i+=2){
                RPNodeEditor startNode=GetNode(mCurrentEditPipelineData.Links[0]) as RPNodeEditor;
                RPNodeEditor endNode=GetNode(mCurrentEditPipelineData.Links[1]) as RPNodeEditor;
                LinkEditor link=new LinkEditor(startNode.GetRightConnectionPoint(),endNode.GetLeftConnectionPoint());
                link.mStartRP=startNode.mID;
                link.mEndRP=endNode.mID;
                startNode.mOutgoingLink=link;
                endNode.mIncomingLink=link;
                mLinks.Add(link);
            }
        }
        void OnGUI()
        {
            ProcessMouseEvent();
            //draw graphic node
            foreach (Node node in mNodes)
            {
                node.Draw(mEditNameNode==node);
            }
            if (mCurrentSelectedNode != null)
            {
                if (mCurrentSelectedNode.GetType() == typeof(RPNodeEditor))
                {
                    mRPSettingInfo.Draw(position.size, (RPNodeEditor)mCurrentSelectedNode);
                }else if(mCurrentSelectedNode.GetType() == typeof(RTNodeEditor))
                {
                    mRTSettingInfo.Draw(position.size, mCurrentSelectedNode as RTNodeEditor);
                }
               
            }
            if (mbShowCreateOptions)
            {
                ShowCreateOptions();
                mbShowCreateOptions = false;
            }
            //draw links
            foreach (LinkEditor link in mLinks)
            {
                link.Draw(position.size);
            }
            if (mTempLink != null)
            {
                mTempLink.Draw(position.size);
            }
        }
        void OnEditName(Node inNode)
        {
            mEditNameNode = inNode;
        }
        void OnCreateRenderPass(float inX, float inY)
        {
            RPNode rpNode = new RPNodeEditor(inX, inY, 120, 60);
            mNodes.Add(rpNode);
        }
        void OnCreateRenderTarget(float inX, float inY)
        {
            RTNode rtNode = new RTNodeEditor(inX, inY, 100, 40);
            mNodes.Add(rtNode);
        }
        void ProcessMouseEvent()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                mLastMouseButtonDownPosition = Event.current.mousePosition;
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonDown();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonDown();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonDown();
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonUp();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonUp();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonUp();
                }
            }else if (Event.current.type == EventType.MouseDrag)
            {
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonDrag();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonDrag();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonDrag();
                }
            }
        }
        void OnMouseLeftButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    //Debug.Log("on left button down "+ node);
                    break;
                }
            }
            if (touchedNode != null)
            {
                if (Event.current.control)
                {
                    touchedNode.OnEditName();
                    mEditNameNode = touchedNode;
                }
                else if (Event.current.shift)
                {
                    if (touchedNode.IsClickOnTheLeftPart(Event.current.mousePosition.x))
                    {
                        if (touchedNode.mIncomingLink != null)
                        {
                            Link link = touchedNode.mIncomingLink;
                            Node other = GetNode(link.mStartRP);
                            other.mOutgoingLink = null;
                            touchedNode.mIncomingLink = null;
                            mLinks.Remove(link);
                        }
                    }
                    else
                    {
                        if (touchedNode.mOutgoingLink != null)
                        {
                            Link link = touchedNode.mOutgoingLink;
                            Node other = GetNode(link.mEndRP);
                            other.mIncomingLink = null;
                            touchedNode.mOutgoingLink = null;
                            mLinks.Remove(link);
                        }
                    }
                }
                else
                {
                    mCurrentSelectedNode = touchedNode;
                }
            }
        }
        void OnMouseMiddleButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (touchedNode != null)
            {
                mLastMiddleButtonTouchedNode = touchedNode;
                mDragPosition = mLastMouseButtonDownPosition;
                mDragOffsetToLocalOriginalPosition = mDragPosition - touchedNode.mRect.position;
            }
        }
        void OnMouseRightButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (touchedNode == null)
            {
                mbShowCreateOptions = true;
            }
            else
            {

            }
        }
        void OnMouseLeftButtonUp()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (mTempLink != null)
            {
                if (touchedNode != null)
                {
                    if (typeof(RPNodeEditor) == touchedNode.GetType()&&mTempLink.mStartRP!=touchedNode.mID)
                    {
                        mTempLink.mEndRP = touchedNode.mID;
                        touchedNode.mIncomingLink = mTempLink;
                        mTempLink.SetPoint(1, touchedNode.GetLeftConnectionPoint());
                        mLinks.Add(mTempLink);
                    }
                }
                else
                {
                    Node startNode = GetNode(mTempLink.mStartRP);
                    startNode.mOutgoingLink = null;
                }
                GUI.changed = true;
                mTempLink = null;
            }
            else
            {
                if (!mRPSettingInfo.mRect.Contains(Event.current.mousePosition)&&!mRTSettingInfo.mRect.Contains(Event.current.mousePosition))
                {
                    mCurrentSelectedNode = touchedNode;
                    GUI.changed = true;
                }
            }
            if (mEditNameNode != null)
            {
                if (!mEditNameNode.mRect.Contains(Event.current.mousePosition))
                {
                    mEditNameNode.OnFinishedEditName();
                    mEditNameNode = null;
                    GUI.changed = true;
                }
            }
        }
        void OnMouseMiddleButtonUp()
        {
            if (mLastMiddleButtonTouchedNode != null)
            {
                mLastMiddleButtonTouchedNode = null;
            }
        }
        void OnMouseRightButtonUp()
        {
        }
        void OnMouseLeftButtonDrag()
        {
            if (mCurrentSelectedNode != null && mEditNameNode==null)
            {
                if (mTempLink == null && mCurrentSelectedNode.mOutgoingLink == null && mCurrentSelectedNode.GetType()==typeof(RPNodeEditor))
                {
                    float x = mCurrentSelectedNode.mRect.xMax;
                    float y = mCurrentSelectedNode.mRect.yMin + mCurrentSelectedNode.mRect.height / 2;
                    mTempLink = new LinkEditor(new Vector3(x, y, 0.0f), new Vector3(x, y, 0.0f));
                    mTempLink.mStartRP = mCurrentSelectedNode.mID;
                    mCurrentSelectedNode.mOutgoingLink = mTempLink;
                }
                else
                {
                    if (mTempLink != null)
                    {
                        mTempLink.SetPoint(1, Event.current.mousePosition);
                        GUI.changed = true;
                    }
                }
            }
        }
        void OnMouseRightButtonDrag()
        {
        }
        void OnMouseMiddleButtonDrag()
        {
            if (mLastMiddleButtonTouchedNode != null)
            {
                ProcessDragNode();
            }
        }
        void ShowCreateOptions()
        {
            GUILayout.BeginArea(new Rect(10, 10, position.width - 20, position.height - 20));
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create Render Pass"), false, () => { OnCreateRenderPass(mLastMouseButtonDownPosition.x, mLastMouseButtonDownPosition.y); });
            menu.AddItem(new GUIContent("Create Render Target"), false, () => { OnCreateRenderTarget(mLastMouseButtonDownPosition.x, mLastMouseButtonDownPosition.y); });
            menu.ShowAsContext();
            GUILayout.EndArea();
        }
        void ProcessDragNode()
        {
            Vector2 draggingOffset = Event.current.mousePosition - mDragPosition;
            Vector2 newPosition = mDragPosition + draggingOffset - mDragOffsetToLocalOriginalPosition;
            mLastMiddleButtonTouchedNode.mRect.position = newPosition;
            mDragPosition = Event.current.mousePosition;
            if (mLastMiddleButtonTouchedNode.mOutgoingLink != null)
            {
                mLastMiddleButtonTouchedNode.mOutgoingLink.SetPoint(0, mLastMiddleButtonTouchedNode.GetRightConnectionPoint());
            }
            if (mLastMiddleButtonTouchedNode.mIncomingLink != null)
            {
                mLastMiddleButtonTouchedNode.mIncomingLink.SetPoint(1, mLastMiddleButtonTouchedNode.GetLeftConnectionPoint());
            }
            GUI.changed = true;
        }
        Node GetNode(string inID)
        {
            foreach(Node node in mNodes)
            {
                if (node.mID.CompareTo(inID)==0) {
                    return node;
                }
            }
            return null;
        }
        void OnDisable(){
            //save current editing pipeline data
            mCurrentEditPipelineData.RenderPasses.Clear();
            mCurrentEditPipelineData.RenderPassIDs.Clear();
            mCurrentEditPipelineData.RenderPassRects.Clear();
            mCurrentEditPipelineData.RenderTargets.Clear();
            mCurrentEditPipelineData.RenderTargetIDs.Clear();
            mCurrentEditPipelineData.RenderTargetRects.Clear();
            mCurrentEditPipelineData.RenderTargetFormats.Clear();
            mCurrentEditPipelineData.RenderPassColor0Outputs.Clear();
            mCurrentEditPipelineData.RenderPassDSOutputs.Clear();
            mCurrentEditPipelineData.RenderPassOnEnterActions.Clear();
            mCurrentEditPipelineData.RenderPassColor0LoadActions.Clear();
            mCurrentEditPipelineData.RenderPassColor0StoreActions.Clear();
            mCurrentEditPipelineData.RenderPassDSLoadActions.Clear();
            mCurrentEditPipelineData.RenderPassDSStoreActions.Clear();
            mCurrentEditPipelineData.RenderPassLightModes.Clear();
            mCurrentEditPipelineData.RenderPassQueues.Clear();
            mCurrentEditPipelineData.RenderPassLayerMasks.Clear();
            mCurrentEditPipelineData.RenderPassScriptNames.Clear();
            mCurrentEditPipelineData.Links.Clear();
            foreach(Node node in mNodes){
                if(node.GetType()==typeof(RPNodeEditor)){
                    RPNodeEditor rpNode=node as RPNodeEditor;
                    mCurrentEditPipelineData.RenderPasses.Add(rpNode.mName);
                    mCurrentEditPipelineData.RenderPassIDs.Add(rpNode.mID);
                    mCurrentEditPipelineData.RenderPassRects.Add(rpNode.mRect);
                    mCurrentEditPipelineData.RenderPassColor0Outputs.Add(rpNode.mOutputColor0);
                    mCurrentEditPipelineData.RenderPassDSOutputs.Add(rpNode.mOutputDS);
                    mCurrentEditPipelineData.RenderPassQueues.Add(rpNode.mQueue);
                    mCurrentEditPipelineData.RenderPassLayerMasks.Add(rpNode.mLayerMask);
                    mCurrentEditPipelineData.RenderPassScriptNames.Add(rpNode.mAttachedScriptName);
                    mCurrentEditPipelineData.RenderPassOnEnterActions.Add(rpNode.mEnterRenderPassAction);
                    mCurrentEditPipelineData.RenderPassColor0LoadActions.Add(rpNode.mColorRTLoadAction);
                    mCurrentEditPipelineData.RenderPassColor0StoreActions.Add(rpNode.mColorRTStoreAction);
                    mCurrentEditPipelineData.RenderPassDSLoadActions.Add(rpNode.mDSRTLoadAction);
                    mCurrentEditPipelineData.RenderPassDSStoreActions.Add(rpNode.mDSRTStoreAction);
                    foreach(string lightMode in rpNode.mLightModes){
                        mCurrentEditPipelineData.RenderPassLightModes.Add(rpNode.mID+'+'+lightMode);
                    }
                }else{
                    RTNodeEditor rtNode=node as RTNodeEditor;
                    mCurrentEditPipelineData.RenderTargets.Add(rtNode.mName);
                    mCurrentEditPipelineData.RenderTargetIDs.Add(rtNode.mID);
                    mCurrentEditPipelineData.RenderTargetRects.Add(rtNode.mRect);
                    mCurrentEditPipelineData.RenderTargetFormats.Add(rtNode.mFormat);
                }
            }
            mCurrentEditPipelineData.Links=new List<string>();
            foreach(Link link in mLinks){
                mCurrentEditPipelineData.Links.Add(link.mStartRP);
                mCurrentEditPipelineData.Links.Add(link.mEndRP);
            }
            EditorUtility.SetDirty(mCurrentEditPipelineData);
            AssetDatabase.SaveAssets();
        }
    }
}
