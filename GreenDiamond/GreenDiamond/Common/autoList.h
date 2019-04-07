/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
template <class Element_t>
class autoList
{
private:
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	int Count;
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	int ListSize;
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t *List;

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Init(Element_t *list, int listSize, int count)
	{
		this->Count = count;
		this->ListSize = listSize;
		this->List = list;
	}

public:
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList()
	{
		this->Init((Element_t *)memAlloc(0), 0, 0);
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList(int buffer_size)
	{
		errorCase(buffer_size < 0 || IMAX / sizeof(Element_t) < buffer_size);

		this->Init((Element_t *)memAlloc(buffer_size * sizeof(Element_t)), buffer_size, 0);
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList(Element_t *list_bind, int count)
	{
		errorCase(!list_bind);
		errorCase(count < 0 || IMAX < count);

		this->Init(list_bind, count, count);
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList(const autoList &source)
	{
		error();
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	~autoList()
	{
		memFree(this->List);
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList<Element_t> *GetClone()
	{
		autoList<Element_t> *list_ret = new autoList<Element_t>();

		list_ret->Count = this->Count;
		list_ret->ListSize = this->Count;
		list_ret->List = (Element_t *)memClone(this->List, this->Count * sizeof(Element_t));

		return list_ret;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	autoList<Element_t> *Molt()
	{
		autoList<Element_t> *list_ret = new autoList<Element_t>();

		list_ret->Count = this->Count;
		list_ret->ListSize = this->ListSize;
		list_ret->List = this->List;

		this->Count = 0;
		this->ListSize = 0;
		this->List = (Element_t *)memAlloc(0);

		return list_ret;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t *UnbindBuffer()
	{
		Element_t *list_ret = this->List;

		this->Count = 0;
		this->ListSize = 0;
		this->List = (Element_t *)memAlloc(0);

		return list_ret;
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Clear()
	{
		this->Count = 0;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void SetCount(int count)
	{
		errorCase(count < 0 || this->Count < count);
		this->Count = count;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	int GetCount()
	{
		return this->Count;
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t *ElementAt(int index)
	{
		if(index == 0) // this->List ���_�C���N�g�ɎQ�Ƃ��邽�߂̓���
			return this->List;

		errorCase(index < 0 || this->Count <= index);

		return this->List + index;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void SetElement(int index, Element_t element)
	{
		errorCase(index < 0 || this->Count <= index);

		this->List[index] = element;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t GetElement(int index)
	{
		errorCase(index < 0 || this->Count <= index);

		return this->List[index];
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	/*
		������ count �� AddElement() ���Ă��o�b�t�@�̊g�����N����Ȃ��悤�Ƀo�b�t�@���g������B
	*/
	void BufferExtend(int count)
	{
		errorCase(count < 0 || IMAX - this->Count < count);

		int reqListSize = this->Count + count;

		if(this->ListSize < reqListSize)
		{
			errorCase(IMAX / sizeof(Element_t) < reqListSize);

			this->ListSize = reqListSize;
			this->List = (Element_t *)memRealloc(this->List, this->ListSize * sizeof(Element_t));
		}
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void AddElement(Element_t element)
	{
		if(this->ListSize <= this->Count)
		{
			errorCase(this->ListSize != this->Count); // 2bs

			if(this->ListSize < 16)
				this->ListSize += 2;
			else
				this->ListSize *= 2;

			errorCase(this->ListSize <= this->Count); // 2bs
			errorCase(IMAX / sizeof(Element_t) < this->ListSize);

			this->List = (Element_t *)memRealloc(this->List, this->ListSize * sizeof(Element_t));
		}
		this->List[this->Count] = element;
		this->Count++;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t UnaddElement()
	{
		errorCase(this->Count < 1);

		this->Count--;
		return this->List[this->Count];
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void InsertElement(int index, Element_t element)
	{
		errorCase(index < 0 || this->Count < index);

		this->AddElement(element); // dummy

		for(int i = this->Count - 1; index < i; i--)
		{
			this->List[i] = this->List[i - 1];
		}
		this->List[index] = element;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t DesertElement(int index)
	{
		errorCase(index < 0 || this->Count <= index);

		Element_t element = this->List[index];

		this->Count--;

		for(int i = index; i < this->Count; i++)
		{
			this->List[i] = this->List[i + 1];
		}
		return element;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t FastDesertElement(int index)
	{
		errorCase(index < 0 || this->Count <= index);

		Element_t element = this->List[index];

		this->Count--;
		this->List[index] = this->List[this->Count];

		return element;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void RemoveElements(int start, int count)
	{
		errorCase(start < 0 || this->Count < start);
		errorCase(count < 0 || this->Count - start < count);

		int index;

		for(index = start; index + count < this->Count; index++)
		{
			this->List[index] = this->List[index + count];
		}
		this->Count = index;
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void CallAllElement(void (*func)(Element_t e))
	{
		for(int index = 0; index < this->Count; index++)
		{
			func(this->GetElement(index));
		}
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Clear(void (*func)(Element_t e))
	{
		this->CallAllElement(func);
		this->Clear();
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void AddElements(Element_t *list, int count)
	{
		this->BufferExtend(count);

		for(int index = 0; index < count; index++)
		{
			this->AddElement(list[index]);
		}
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void AddElements(autoList<Element_t> *list)
	{
		this->AddElements(list->ElementAt(0), list->GetCount());
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Overwrite(autoList<Element_t> *list)
	{
		this->Clear();
		this->AddElements(list);
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Swap(int index1, int index2)
	{
		errorCase(index1 < 0 || this->Count <= index1);
		errorCase(index2 < 0 || this->Count <= index2);

		Element_t tmp = this->List[index1];

		this->List[index1] = this->List[index2];
		this->List[index2] = tmp;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Reverse()
	{
		int i = 0;
		int j = this->Count - 1;

		while(i < j)
		{
			this->Swap(i, j);
			i++;
			j--;
		}
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void PutElement(int index, Element_t element, Element_t defaultElement)
	{
		errorCase(index < 0 || IMAX < index);

		if(this->Count <= index)
		{
			while(this->Count < index)
			{
				this->AddElement(defaultElement);
			}
			this->AddElement(element);
		}
		else
			this->SetElement(index, element);
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	Element_t RefElement(int index, Element_t defaultElement)
	{
		if(m_isRange(index, 0, this->Count - 1))
		{
			return this->GetElement(index);
		}
		return defaultElement;
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	int UnaddRepeat(Element_t e)
	{
		int num = 0;

		while(this->Count && this->List[this->Count - 1] == e)
		{
			this->Count--;
			num++;
		}
		return num;
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void AddRepeat(Element_t e, int num)
	{
		for(int c = 0; c < num; c++)
		{
			this->AddElement(e);
		}
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void MultiDiscard(int (*isDiscardPosFunc)(Element_t *ePos))
	{
		int start;

		for(start = 0; start < this->Count; start++)
			if(isDiscardPosFunc(this->List + start))
				break;

		if(start == this->Count)
			return;

		int rPos = start + 1;
		int wPos = start;

		while(rPos < this->Count)
		{
			if(!isDiscardPosFunc(this->List + rPos))
			{
				this->List[wPos] = this->List[rPos];
				wPos++;
			}
			rPos++;
		}
		this->Count = wPos;
	}

	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void GnomeSort(int (*compFunc)(Element_t, Element_t), int start, int count)
	{
		for(int index = 1; index < count; index++)
		{
			for(int nearPos = index - 1; 0 <= nearPos; nearPos--)
			{
				if(compFunc(this->List[start + nearPos], this->List[start + nearPos + 1]) <= 0)
					break;

				this->Swap(start + nearPos, start + nearPos + 1);
			}
		}
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Sort(int (*compFunc)(Element_t, Element_t), int start, int count)
	{
		int span = count;

		for(; ; )
		{
			span *= 10;
			span /= 13;

			if(span < 2)
				break;

			if(span == 9 || span == 10)
				span = 11;

			for(int index = 0; index + span < count; index++)
				if(0 < compFunc(this->List[start + index], this->List[start + index + span]))
					this->Swap(start + index, start + index + span);
		}
		this->GnomeSort(compFunc, start, count);
	}
	/*
		copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
	*/
	void Sort(int (*compFunc)(Element_t, Element_t))
	{
		this->Sort(compFunc, 0, this->Count);
	}
};

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
template <class Element_t>
void releaseList(autoList<Element_t> *list, void (*func)(Element_t e))
{
	errorCase(!func);

	if(!list)
		return;

	list->CallAllElement(func);
	delete list;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
template <class Element_t>
Element_t *unbindBlock(autoList<Element_t> *list)
{
	Element_t *block = list->UnbindBuffer();
	delete list;
	return block;
}

/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *unbindBlock2Line(autoList<char> *list);
/*
	copied the source file by https://github.com/stackprobe/Factory/blob/master/SubTools/CopyLib.c
*/
char *unbindBlock2Line_NR(autoList<char> *list);
